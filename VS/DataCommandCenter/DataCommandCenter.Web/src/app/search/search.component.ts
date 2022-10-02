import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ObjectSearch, SearchObjectTypes, ServerDTO } from "../models/MetadataDTOs";
import { SearchService } from "./search-service.module";
import { Network, DataSet } from 'vis';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit, AfterViewInit {
  servers: ServerDTO[] = [];
  searchResult: ObjectSearch[] = [];
  searchSettings: SearchObjectTypes = { QueryString: "", Servers: false, Databases: true, Tables:true, Views: true, ProgrammableObjects: true, Columns: true };
    
  sub!: Subscription;
  errorMessage = '';
  keyword = "displayText";

  @ViewChild('network') el!: ElementRef;
  private networkInstance: any;

  constructor(private searchService: SearchService) { }

  ngOnInit(): void {
    //this.getServers();
    this.doSearch('');
  }

  ngAfterViewInit() {
    var svg =
      '<svg xmlns="http://www.w3.org/2000/svg" width="390" height="65">' +
      '<rect x="0" y="0" width="100%" height="100%" fill="#7890A7" stroke-width="20" stroke="#ffffff" ></rect>' +
      '<foreignObject x="15" y="10" width="100%" height="100%">' +
      '<div xmlns="http://www.w3.org/1999/xhtml" style="font-size:40px">' +
      " <em>I</em> am" +
      '<span style="color:white; text-shadow:0 0 20px #000000;">' +
      " HTML in SVG!</span>" +
      "</div>" +
      "</foreignObject>" +
      "</svg>";

    var svg2 =
      `<svg xmlns="http://www.w3.org/2000/svg" width="390" height="65">
<foreignObject x="15" y="10" width="100%" height="100%">
  <div xmlns="http://www.w3.org/1999/xhtml"><img src="https://localhost:4200/assets/images/sql.jpg"></img>this is my stuff</div>
</foreignObject>
       </svg>`;
    
    var url = "data:image/svg+xml;charset=utf-8," + encodeURIComponent(svg2);

    const container = this.el.nativeElement;
    const nodes = new DataSet<any>([
      { id: "t1", image: url, shape: "image" },
      { id: "t2", image: 'assets/images/sql.jpg', label: 'Node 2', shape: "image" },
      { id: "t3", label: 'Node 3' },
      { id: "t4", label: 'Node 4' },
      { id: "t5", label: 'Node 5' }
    ]);

    const edges = new DataSet<any>([
      { from: "t1", to: "t3" },
      { from: "t1", to: "t2" },
      { from: "t2", to: "t4" },
      { from: "t2", to: "t5" }
    ]);
    const data = { nodes, edges };

    const options = {};

    this.networkInstance = new Network(container, data, options);
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

    alert(item.displayText);
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
