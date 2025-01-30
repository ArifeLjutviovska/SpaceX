import { Component, OnInit } from '@angular/core';
import { SpacexService } from './services/spacex.services';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [FormsModule]
})
export class AppComponent{

  constructor(private spacexService: SpacexService){
  }
 
  inputValue: string = '';
  textValue: string = ''; 

  createUser() {
    this.spacexService.createUser(this.inputValue).subscribe(result =>{
      if (result.isSuccess){
        this.textValue = "User created with name: " + result.value;
      }else{
        this.textValue = " USER NOT CREATED";
      }
    })
  }

}
