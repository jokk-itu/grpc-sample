import { Component } from '@angular/core';
import { loadPackageDefinition, GrpcObject, credentials } from '@grpc/grpc-js';
import { PackageDefinition, loadSync } from '@grpc/proto-loader';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  title = 'Client';
}

const weatherProtoPath: string = __dirname + 'weather.proto';
const weatherDefinition: PackageDefinition = loadSync(weatherProtoPath);
const weatherProto: any = loadPackageDefinition(weatherDefinition);
const weatherService: any = weatherProto.weather.Weather('localhost:5242', credentials.createInsecure());

let weathers: any[] = [];
let country: string = '';

function getWeather(): any {
  weathers = [];
  let call = weatherService.getWeather({});
  call.on('data', (response: any) => {
    weathers.push(response);
    console.log(response);
  });
  call.on('end', () => console.log('Call ended'));
  call.on('error', (response: any) => console.log('Error occurred: ' + response));
  call.on('status', (response: any) => console.log('Received status: ' + response));
}

function getCountryWeather(): any {
  weathers = [];
  weatherService.getCountryWeather({ country: country }, (error: any, response: any) => {
    if(error){
      console.error(error);
    } else {
      console.log(response);
      weathers.push(response);
    }
  });
}