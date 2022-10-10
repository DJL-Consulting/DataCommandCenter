import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ColumnDTO, DatabaseDTO, MetadataDTO, ObjectDTO, ObjectSearch, SearchObjectTypes, ServerDTO } from "../models/MetadataDTOs";
import { IntegrationDTO, LineageDTO, LineageLink, LineageNode } from "../models/LineageDTO";
import { PropertyDTO } from "../models/PropertyDTO";
import { SearchService } from "./search-service.module";
import { Network } from 'vis-network';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { DataSet } from "vis-data/peer/esm/vis-data"


@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit, AfterViewInit {
  servers: ServerDTO[] = [];
  searchResult: ObjectSearch[] = [];
  lineagedata: LineageDTO = { nodes: [], flows: [] };
  metadata: MetadataDTO = { servers: [], databases: [], objects: [], columns: [] };
  defaultSearchSettings: SearchObjectTypes = { SearchType: "Metadata", QueryString: "", Servers: false, Databases: true, Tables: true, Views: true, ProgrammableObjects: true, Columns: true, Integrations: false };
  lineageSearchSettings: SearchObjectTypes = { SearchType: "Lineage", QueryString: "", Servers: false, Databases: false, Tables: true, Views: true, ProgrammableObjects: true, Columns: false, Integrations: true };
  searchSettings: SearchObjectTypes = this.defaultSearchSettings;
  searchLineage: boolean = false;
  searchText: string = "";
  lineageRow?: LineageNode = undefined;

  flowRow?: LineageLink = undefined;

  hasSearch: boolean = false;
  firstDraw: boolean = false;

  lastclick: number = new Date().getTime();
  doubleclick: boolean = false;
  serverRow?: ServerDTO;
  dbRow?: DatabaseDTO;
  objRow?: ObjectDTO;
  colRow?: ColumnDTO;

  @ViewChild('serverModal') sdialog!: ElementRef;
  @ViewChild('databaseModal') ddialog!: ElementRef;
  @ViewChild('objectModal') odialog!: ElementRef;
  @ViewChild('columnModal') cdialog!: ElementRef;
  @ViewChild('lineageModal') ldialog!: ElementRef;
  @ViewChild('integrationModal') fdialog!: ElementRef;
  @ViewChild('network') el!: ElementRef;
  @ViewChild('popupTable') tablePop!: ElementRef;
  @ViewChild('popupServer') serverPop!: ElementRef;
  @ViewChild('popupDatabase') databasePop!: ElementRef;
  @ViewChild('popupObject') objectPop!: ElementRef;
  @ViewChild('popupColumn') columnPop!: ElementRef;
  @ViewChild('popupIntegration') integrationPop!: ElementRef;
  
  sub!: Subscription;
  errorMessage = '';
  keyword = "displayText";
  searchItem: ObjectSearch = {
      objectType: '',
      id: 0,
      searchText: '',
      displayText: ''
  };

  private networkInstance: any;

  constructor(private searchService: SearchService, private modalService: NgbModal) { }

  async open(content: any) {
    await new Promise(resolve => setTimeout(resolve, 500));

    if (this.doubleclick)
      return;
    this.modalService.open(content, {animation: true, centered: true, size: "md", keyboard: true }).result;
  }

  closeModals() {
    this.modalService.dismissAll(); 
  }

  ngOnInit(): void {
    //this.getServers();
    this.doSearch('');
  }

  searchOptionChange(evt: any) {
    this.searchLineage = !this.searchLineage;
    if (this.searchLineage) {
      this.searchSettings = this.lineageSearchSettings;
    }
    else {
      this.searchSettings = this.defaultSearchSettings;
    }

    if (this.hasSearch)
      this.searchSelectEvent(this.searchItem);
    else
      this.searchChangeSearch(this.searchText);

  }

  ngAfterViewInit() {
  }

  getServers(): void {
    this.sub = this.searchService.getServers().subscribe({
      next: res => {        
        this.servers = res;
      },
      error: err => this.errorMessage = err
    });
  }

  doSearch(query: string): void {
    if (query.length < 1) {
      this.searchResult = [];
      return;
    }

      this.sub = this.searchService.getSearchResults(query, this.searchSettings).subscribe({
        next: res => {
          this.searchResult = res;
        },
        error: err => this.errorMessage = err
      });
  }

  searchFilter = function (items: any[], query: string): any[] {
    return items;
  }

  searchSelectEvent(item: ObjectSearch) {
    this.hasSearch = true;

    this.searchItem = item;
    if (this.searchLineage) {
      this.sub = this.searchService.getLineageForObject(item).subscribe({
        next: res => {
          this.lineagedata = res;
          this.drawLineagedata();
        },
        error: err => this.errorMessage = err
      });
    }
    else {
      this.sub = this.searchService.getMetadataForObject(item).subscribe({
        next: res => {
          this.metadata = res;
          this.drawMetadata();
        },
        error: err => this.errorMessage = err
      });
    }
  }

  searchChangeSearch(search: string) {
    this.hasSearch = false;
    this.searchText = search;
    this.doSearch(search);
  }

  formatDisplay(item: ObjectSearch): string {
    return " "+item.objectType + ' ' + item.displayText;
  }

  setClass(item: ObjectSearch): string {
    var baseClass = "fa fa-lg ";

    var objType = item.objectType.toUpperCase();

    if (objType.includes("SERVER"))
      return baseClass + "fa-server";

    if (objType.includes("DATABASE"))
      return baseClass + "fa-database";

    if (objType.includes("TABLE") || item.objectType.includes("VIEW"))
      return baseClass + "fa-table";

    if (objType.includes("COLUMN"))
      return baseClass + "fa-pencil";

    if (objType.includes("SQL"))
      return baseClass + "fa-gear";

    if (objType.includes("INTEGRATION"))
      return baseClass + "fa-copy";


    return "fa-objects-column";

  }

  ShowSearchOptions() {

  }

  searchFocused(e: Event) {

  }

  pickImage(objType: string): any {
    switch (objType.toUpperCase()) {
      case "USER_TABLE":
        return { unselected: 'assets/images/table.png', selected: 'assets/images/table-sel.png' };
      case "VIEW":
        return { unselected: 'assets/images/view.png', selected:'assets/images/view-sel.png'  };
      case "FILE":
        return { unselected: 'assets/images/file.png', selected: 'assets/images/file-sel.png' };
      case "":
        return { unselected: 'assets/images/', selected: 'assets/images/' };
    }

    return { unselected: 'assets/images/proc.png', selected: 'assets/images/proc-sel.png' };
  }

  drawLineagedata(): void {
    const tablePopElement = this.tablePop.nativeElement;

    const container = this.el.nativeElement;
    const integrationPopElement = this.integrationPop.nativeElement;

    const arrowColor = { color: "#97C2FC", opacity: 0.4, highlight: "#00CC00" };  // color: "#97C2FC", highlight: "red"

    var sid = ((this.searchItem?.objectType.toUpperCase() == "INTEGRATION") ? "i" : "o") + this.searchItem?.id.toString();

    var maxItems = this.lineagedata.nodes.length;

    var nodes = new DataSet<any>();
    var edges = new DataSet<any>();

    var popup = document.getElementById("popupTable");

    var ids: number[] = [];

    var ct = this;

    console.log(this.lineagedata.nodes);

    this.lineagedata.nodes.forEach(function (obj) {
      if (!ids.includes(obj.id))
        nodes.add({ title: tablePopElement, id: "o" + obj.id.toString(), level: obj.level, label: obj.title, widthConstraint: 200, image: ct.pickImage(obj.objectType == null ? '' : obj.objectType), shape: "image", size: 30, chosen: ("o" + obj.id.toString() == sid) })

      ids.push(obj.id);
    });

    var cnt = 0;
    this.lineagedata.flows.forEach(function (obj) {
      edges.add({ id: cnt++, to: "o" + obj.sourceObjectId?.toString(), from: "o" + obj.destinationObjectId?.toString(), arrows: "from", title: obj.integrationFlowId == null ? obj.operation : integrationPopElement, color: obj.integrationFlowId == null ? arrowColor : "#FFFF00" }) //label: obj.integrationInfo,
    });

    const data = { nodes, edges };

    if (this.searchItem?.objectType.toUpperCase() == "INTEGRATION") {
      var options = {
        autoResize: false,
        interaction: { hover: true, tooltipDelay: 300 },
        layout: {
          randomSeed: undefined,
          clusterThreshold: 150,
          improvedLayout: false,
          hierarchical: {
            enabled: true,
            levelSeparation: 150,
            nodeSpacing: 100,
            treeSpacing: 100,
            blockShifting: true,
            edgeMinimization: false,
            parentCentralization: true,
            direction: 'LR', // UD, DU, LR, RL
            sortMethod: 'hubsize', //hubsize, directed
            shakeTowards: 'leaves' //roots, leaves
          }
        }
      };
    }
    else {
      var options = {
        autoResize: false,
        interaction: { hover: true, tooltipDelay: 300 },
        layout: {
          randomSeed: undefined,
          clusterThreshold: 150,
          improvedLayout: false,
          hierarchical: {
            enabled: true,
            levelSeparation: 150,
            nodeSpacing: 100,
            treeSpacing: 1000,
            blockShifting: true,
            edgeMinimization: false,
            parentCentralization: true,
            direction: 'LR', // UD, DU, LR, RL
            sortMethod: 'directed', //hubsize, directed
            shakeTowards: 'leaves' //roots, leaves
          }
        }
      };
    }

    this.networkInstance = new Network(container, data, options);

    //if (this.networkInstance != null) {
    //  this.networkInstance.setOptions(options);
    //  this.networkInstance.setData({ nodes: data.nodes, edges: data.edges });
    //}
    //else {
    //  this.networkInstance = new Network(container, data, options);
    //}

    var w = 1000;
    if (this.searchItem?.objectType.toUpperCase() != "INTEGRATION")
      this.networkInstance.setSelection({ nodes: [sid] });


    this.firstDraw = true;
    var ctx = this;
    this.networkInstance.on("doubleClick", function (params: any) {
      ctx.doubleclick = true;
      if (params.nodes.length) {
        // Get the lienage for the object
        var newSearchItem = {
          objectType: "SQL Table",
          id: params.nodes[0].substring(1),
          searchText: "Search Text",
          displayText: "Display Text"
        };
        ctx.searchSelectEvent(newSearchItem);
      }
    });

    this.networkInstance.on("afterDrawing", function (canvas: any) {
      if (!ctx.firstDraw)
        return;

      ctx.firstDraw = false;

      var zoptions = {
        scale: 1.25,
        animation: {
          duration: 1000,
          easingFunction: "easeInOutQuad"
        }
      };

      if (ctx.searchItem?.objectType.toUpperCase() != "INTEGRATION")
        ctx.networkInstance.focus(sid, zoptions);
    });

    this.networkInstance.on("hoverNode", function (params: any) {
      var selId = params.node;
      var row = ctx.lineagedata.nodes.find((obj) => { return 'o' + obj.id.toString() === selId; });

      ctx.lineageRow = row;
   });

    this.networkInstance.on("hoverEdge", function (params: any) {
      var selId = params.edge;

      var row = ctx.lineagedata.flows[selId];

      ctx.flowRow = row;
    });


    var nds = this.lineagedata.nodes;

    //was on("click",....
    this.networkInstance.on("oncontext", function (params: any) {
      params.event.preventDefault();

      var d = new Date();

      if (d.getTime() - ctx.lastclick < 1000) 
        return;

      ctx.doubleclick = false;

      ctx.lastclick = d.getTime();

      if (params.nodes.length) {
        var selId = params.nodes[0];
        var row = ctx.lineagedata.nodes.find((obj) => { return 'o' + obj.id.toString() === selId; });

        ctx.lineageRow = row;

        ctx.open(ctx.ldialog);
        return;
      }
      if (params.edges.length) {
        var selId = params.edges[0];

        var erow = ctx.lineagedata.flows[selId];

        ctx.flowRow = erow;
        ctx.open(ctx.fdialog);
      }
    });
  }

  drawMetadata(): void {
    const container = this.el.nativeElement;

    const arrowColor = { color: "#97C2FC", opacity: 0.4, highlight: "#00CC00" };  // color: "#97C2FC", highlight: "red"

    var nodes = new DataSet<any>();
    var edges = new DataSet<any>();

    var maxWidth = 100;

    var maxItems = 0;
    maxItems = (this.metadata.servers.length > maxItems) ? this.metadata.servers.length : maxItems;
    maxItems = (this.metadata.databases.length > maxItems) ? this.metadata.databases.length : maxItems;
    maxItems = (this.metadata.objects.length > maxItems) ? this.metadata.objects.length : maxItems;
    maxItems = (this.metadata.columns.length > maxItems) ? this.metadata.columns.length : maxItems;

    var sid = "o";
    if (this.searchItem?.objectType.toUpperCase() == "SERVER")
      sid = "s";
    if (this.searchItem?.objectType.toUpperCase() == "DATABASE")
      sid = "d";
    if (this.searchItem?.objectType.toUpperCase() == "SQL COLUMN")
      sid = "c";

    sid = sid + this.searchItem?.id.toString();

    var ct = this;

    this.metadata.servers.forEach(function (obj) {
      nodes.add({ id: "s" + obj.id.toString(), title: ct.serverPop.nativeElement, level: 1, label: obj.serverName, widthConstraint: maxWidth, image: { unselected: 'assets/images/server.png', selected: 'assets/images/server-sel.png' }, shape: "image", size: 30, chosen: ("s" + obj.id.toString() == sid) })
    });

    this.metadata.databases.forEach(function (obj) {
      nodes.add({ id: "d" + obj.id.toString(), title: ct.databasePop.nativeElement, level: 2, label: obj.databaseName, widthConstraint: maxWidth, image: { unselected: 'assets/images/db2.png', selected: 'assets/images/db2-sel.png' }, shape: "image", size: 30, chosen: ("d" + obj.id.toString() == sid) })
      edges.add({ from: "d" + obj.id.toString(), to: "s" + obj.serverId?.toString(), arrows: "to", color: arrowColor })
    });

    this.metadata.objects.forEach(function (obj) {
      //nodes.add({ id: "o" + obj.id.toString(), label: obj.objectName, widthConstraint: 400, shape: "rectangle", size: 30 })
      nodes.add({ id: "o" + obj.id.toString(), title: ct.objectPop.nativeElement, level: 3, label: obj.objectName, widthConstraint: maxWidth, image: ct.pickImage(obj.objectType == null ? '' : obj.objectType), shape: "image", size: 30, chosen: ("o" + obj.id.toString() == sid) })
      edges.add({ from: "o" + obj.id.toString(), to: "d" + obj.databaseId?.toString(), arrows: "to", color: arrowColor })
    });

    this.metadata.columns.forEach(function (obj) {
      nodes.add({ id: "c" + obj.id.toString(), title: ct.columnPop.nativeElement, level: 4, label: obj.columnName, widthConstraint: maxWidth, image: { unselected: 'assets/images/column.png', selected: 'assets/images/column-sel.png' }, shape: "image", size: 30, chosen: ("c" + obj.id.toString() == sid) })
      edges.add({ from: "c" + obj.id.toString(), to: "o" + obj.objectId?.toString(), arrows: "to", color: arrowColor })
    });

    const data = { nodes, edges };

    var options = {
      autoResize: false,
      interaction: { hover: true },
      layout: {
        clusterThreshold: 150,
        improvedLayout: true,
        hierarchical: {
          enabled: true,
          levelSeparation: 300,
          nodeSpacing: 200,
          treeSpacing: 1000,
          blockShifting: true,
          edgeMinimization: false,
          parentCentralization: true,
          direction: 'UD', // UD, DU, LR, RL
          sortMethod: 'directed' //hubsize, directed
          //shakeTowards: 'leaves' //roots, leaves

          //direction: "UD",
        },
      },
      physics: {
        hierarchicalRepulsion: {
          nodeDistance: (100 + (maxItems * 3)),
        }
      }
    };

    this.networkInstance = new Network(container, data, options);

    //if (this.networkInstance != null) {
    //  this.networkInstance.setOptions(options);
    //  this.networkInstance.setData({ nodes: data.nodes, edges: data.edges });
    //}
    //else {
    //  this.networkInstance = new Network(container, data, options);
    //}

    var w = 1000;
    this.networkInstance.setSelection({ nodes: [sid] });


    var ctx = this;
    ctx.firstDraw = true;

    this.networkInstance.on("afterDrawing", function (canvas: any) {
      if (!ctx.firstDraw)
        return;

      ctx.firstDraw = false;

      var zoptions = {
        scale: 0.75,
        animation: {
          duration: 1000,
          easingFunction: "easeInOutQuad"
        }
      };

      ctx.networkInstance.focus(sid, zoptions);
    });


    this.networkInstance.on("doubleClick", function (params: any) {
      ctx.doubleclick = true;
      ctx.closeModals();

      if (params.nodes.length) {
        switch (params.nodes[0][0]) {
          case "s":
            var newSearchItem = {
              objectType: "Server",
              id: params.nodes[0].substring(1),
              searchText: "Search Text",
              displayText: "Display Text"
            };
            ctx.searchSelectEvent(newSearchItem);
            break;
          case "d":
            var newSearchItem = {
              objectType: "Database",
              id: params.nodes[0].substring(1),
              searchText: "Search Text",
              displayText: "Display Text"
            };
            ctx.searchSelectEvent(newSearchItem);
            break;
          case "c":
            var newSearchItem = {
              objectType: "SQL Column",
              id: params.nodes[0].substring(1),
              searchText: "Search Text",
              displayText: "Display Text"
            };
            ctx.searchSelectEvent(newSearchItem);
            break;
          default:
            var newSearchItem = {
              objectType: "SQL Table",
              id: params.nodes[0].substring(1),
              searchText: "Search Text",
              displayText: "Display Text"
            };
            ctx.searchSelectEvent(newSearchItem);
            break;
          //alert(params.nodes[0][0]);
        }

      }
    });

    this.networkInstance.on("hoverNode", function (params: any) {
      var selId = params.node;
      switch (selId[0]) {
        case "s":
          ctx.serverRow = ctx.metadata.servers.find((obj) => { return 's' + obj.id.toString() === selId; })
          break;
        case "d":
          ctx.dbRow = ctx.metadata.databases.find((obj) => { return 'd' + obj.id.toString() === selId; })
          break;
        case "o":
          ctx.objRow = ctx.metadata.objects.find((obj) => { return 'o' + obj.id.toString() === selId; })
          break;
        case "c":
          ctx.colRow = ctx.metadata.columns.find((obj) => { return 'c' + obj.id.toString() === selId; })
          break;
      }

      //var row = ctx.lineagedata.nodes.find((obj) => { return 'o' + obj.id.toString() === selId; });

      //ctx.lineageRow = row;
    });

    //was on("click",....
    this.networkInstance.on("oncontext", function (params: any) {
      params.event.preventDefault();

      var d = new Date();

      if (d.getTime() - ctx.lastclick < 1000)
        return;

      ctx.doubleclick = false;

      ctx.lastclick = d.getTime();

      if (params.nodes.length) {
        var selId = params.nodes[0];

        switch (selId[0]) {
          case "s":
            ctx.serverRow = ctx.metadata.servers.find((obj) => { return 's' + obj.id.toString() === selId; })
            ctx.open(ctx.sdialog);
            break;
          case "d":
            ctx.dbRow = ctx.metadata.databases.find((obj) => { return 'd' + obj.id.toString() === selId; })
            ctx.open(ctx.ddialog);
            break;
          case "o":
            ctx.objRow = ctx.metadata.objects.find((obj) => { return 'o' + obj.id.toString() === selId; })
            ctx.open(ctx.odialog);
            break;
          case "c":
            ctx.colRow = ctx.metadata.columns.find((obj) => { return 'c' + obj.id.toString() === selId; })
            ctx.open(ctx.cdialog);
            break;
        }
      }
    });


  }


}
