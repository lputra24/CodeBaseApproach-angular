import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-value',
    templateUrl: './value.component.html',
    styleUrls: ['./value.component.css']
})
/** value component*/
export class ValueComponent implements OnInit {
  value: any; //datatype = any 
    
/** value ctor */
    //constructor is too early for making api calls but perfect for dependency injection
  constructor(private http: HttpClient) {

  }

  //once all are initialized we can call api here
  ngOnInit() {
    this.getValues();

  }

  getValues() {
    //observable needs to be subscribed
    this.http.get('http://localhost:5000/value').subscribe(response => {
      this.value = response;
      
      //manage error if something happens while doing http request
    }, error => {
    console.log(error)})
  }
}
