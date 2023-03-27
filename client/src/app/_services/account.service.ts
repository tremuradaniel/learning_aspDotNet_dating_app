import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

// services are singletons - they're instantiated when our application starts 
// and destroyed our app shuts down
// vs ComponentFactoryResolver, which are destroyed along with any stat that is 
// storead inside as we move from component to component
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'htpps://localhost:7092/api/';

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model);
  }
  
}
