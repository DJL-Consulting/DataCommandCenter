import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { MetadataDTO, ObjectSearch, SearchObjectTypes, ServerDTO } from "../models/MetadataDTOs";
import { SearchService } from "./search-service.module";
import { Network } from 'vis-network';

//import { Network } from "vis-network/peer/esm/vis-network";
import { DataSet } from "vis-data/peer/esm/vis-data"

//import { Network } from 'vis-network';
//import { DataSet } from 'vis-data';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit, AfterViewInit {
  servers: ServerDTO[] = [];
  searchResult: ObjectSearch[] = [];
  metadata: MetadataDTO = { servers: [], databases: [], objects: [], columns:[] };
  defaultSearchSettings: SearchObjectTypes = { SearchType: "Metadata", QueryString: "", Servers: false, Databases: true, Tables: true, Views: true, ProgrammableObjects: true, Columns: true };
  lineageSearchSettings: SearchObjectTypes = { SearchType: "Lineage", QueryString: "", Servers: false, Databases: false, Tables: true, Views: true, ProgrammableObjects: true, Columns: false };
  searchSettings: SearchObjectTypes = this.defaultSearchSettings;
  searchLineage: boolean = false;
    
  sub!: Subscription;
  errorMessage = '';
  keyword = "displayText";
  searchItem?: ObjectSearch;

  @ViewChild('network') el!: ElementRef;
  private networkInstance: any;

  constructor(private searchService: SearchService) { }

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
  }

  ngAfterViewInit() {
  }

  getServers(): void {
    this.sub = this.searchService.getServers().subscribe({
      next: res => {
        //console.log(res);
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
          //console.log(res);
          this.searchResult = res;
        },
        error: err => this.errorMessage = err
      });
  }

  searchFilter = function (items: any[], query: string): any[] {
    return items;
  }

  searchSelectEvent(item: ObjectSearch) {
    //alert(item.displayText);
    this.searchItem = item;
    if (this.searchLineage) {
      this.sub = this.searchService.getLineageForObject(item).subscribe({
        next: res => {
          //console.log(res);
          this.metadata = res;
          this.drawMetadata();
        },
        error: err => this.errorMessage = err
      });
    }
    else {
      this.sub = this.searchService.getMetadataForObject(item).subscribe({
        next: res => {
          //console.log(res);
          this.metadata = res;
          this.drawMetadata();
        },
        error: err => this.errorMessage = err
      });
    }
  }

  drawMetadata(): void {
    const container = this.el.nativeElement;

    const arrowColor = { color: "#97C2FC", opacity: 0.4, highlight: "#00CC00" };  // color: "#97C2FC", highlight: "red"

    var nodes = new DataSet<any>();
    var edges = new DataSet<any>();

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

    console.log(sid);

    this.metadata.servers.forEach(function (obj) {
      nodes.add({ id: "s" + obj.id.toString(), label: obj.serverName, widthConstraint: 100, image: 'assets/images/server.jpg', shape: "image", size: 30, chosen: ("s" + obj.id.toString() == sid) })
    });

    this.metadata.databases.forEach(function (obj) {
      nodes.add({ id: "d" + obj.id.toString(), label: obj.databaseName, widthConstraint: 100, image: 'assets/images/db2.png', shape: "image", size: 30, chosen: ("d" + obj.id.toString() == sid) })
      edges.add({ from: "d" + obj.id.toString(), to: "s" + obj.serverId?.toString(), arrows: "to", color: arrowColor })
    });

    this.metadata.objects.forEach(function (obj) {
      //nodes.add({ id: "o" + obj.id.toString(), label: obj.objectName, widthConstraint: 400, shape: "rectangle", size: 30 })
      nodes.add({ id: "o" + obj.id.toString(), label: obj.objectName, widthConstraint: 100, image: obj.objectType == "USER_TABLE" ? 'assets/images/table.jpg' : obj.objectType == "VIEW" ? 'assets/images/view.png' : 'assets/images/proc.png', shape: "image", size: 30, chosen: ("o" + obj.id.toString() == sid) })
      edges.add({ from: "o" + obj.id.toString(), to: "d" + obj.databaseId?.toString(), arrows: "to", color: arrowColor })
    });

    this.metadata.columns.forEach(function (obj) {
      nodes.add({ id: "c" + obj.id.toString(), label: obj.columnName, widthConstraint: 100, image: 'assets/images/column.png', shape: "image", size: 30, chosen: ("c" + obj.id.toString() == sid) })
      edges.add({ from: "c" + obj.id.toString(), to: "o" + obj.objectId?.toString(), arrows: "to", color: arrowColor })
    });

    const data = { nodes, edges };

    //const options = {
    //  layout: {
    //    randomSeed: undefined,
    //    improvedLayout: true,
    //    clusterThreshold: 150,
    //    hierarchial: {
    //      enabled: true,
    //      levelSeparation: 150,
    //      nodeSpacing: 100,
    //      treeSpacing: 1000,
    //      blockShifting: true,
    //      edgeMinimization: false,
    //      parentCentralization: true,
    //      direction: 'UD', // UD, DU, LR, RL
    //      sortMethod: 'hubsize', //hubsize, directed
    //      shakeTowards: 'leaves' //roots, leaves
    //    }
    //  }
    //};


    var options = {
        autoResize: false,
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
            direction: 'DU', // UD, DU, LR, RL
            sortMethod: 'directed' //hubsize, directed
            //shakeTowards: 'leaves' //roots, leaves

            //direction: "UD",
          },
        },
        physics: {
            hierarchicalRepulsion: {
                nodeDistance: (100 + (maxItems * 3)),
            },
        }
      //physics: {
      //  // Even though it's disabled the options still apply to network.stabilize().
      //  enabled: true,
      //  solver: "repulsion",
      //  repulsion: {
      //    nodeDistance: 800 // Put more distance between the nodes.
      //  },
      //  "barnesHut": {
      //    "springConstant": 0,
      //    "avoidOverlap": 0.8
      //  }
      //}
    };



    //var options = {
    //    layout: {
    //      randomSeed: undefined,
    //      improvedLayout: true,
    //      clusterThreshold: 150,
    //      hierarchical: {
    //        enabled: true,
    //        levelSeparation: 150,
    //        nodeSpacing: 100,
    //        treeSpacing: 1000,
    //        blockShifting: true,
    //        edgeMinimization: false,
    //        parentCentralization: true,
    //        direction: 'UD', // UD, DU, LR, RL
    //        sortMethod: 'hubsize', //hubsize, directed
    //        shakeTowards: 'leaves' //roots, leaves

    //        //direction: "UD",
    //      }
    //  }
    //};

    //console.log(data);

    if (this.networkInstance != null) {
      this.networkInstance.destroy();
      this.networkInstance = null;
    }

    this.networkInstance = new Network(container, data, options);

    var w = 1000;
    this.networkInstance.setSelection({ nodes: [sid] });

    var zoptions = {
        scale: 5.0,
        animation: {
            duration: 1000,
            easingFunction: "easeInOutQuad"
        }
    };

      //this.networkInstance.focus(18, { scale: 0.5 })
      this.networkInstance.focus(sid, zoptions);

      //this.networkInstance.moveTo({ scale: 0.001 }) 

    //this.networkInstance.stabilize();
    //this.networkInstance.setSize("8000px", "1000px");

    var ctx = this;
    this.networkInstance.on("doubleClick", function (params:any) {
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
  }

  searchChangeSearch(search: string) {
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


    return "fa-objects-column";

  }

  ShowSearchOptions() {

  }

  searchFocused(e: Event) {

  }

}
