import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Common } from '../common/common';
import { map } from 'rxjs/operators';
import { IpService } from '../ip.service';

// @Injectable({
//   providedIn: 'root'
// })
@Injectable()
export class AuthService {

  constructor(private http: HttpClient, private ipService: IpService) { }
  public authenticate(loginData: any)
  {
    const reqHeader = new HttpHeaders({'No-Auth': 'True'});
    return this.http.post<any>(Common.baseUrl + 'api/auth/login', loginData, {headers: reqHeader})
    .pipe(map((user:any) => {
      //console.log(user);
      if(user != null){
      this.localStorageSet(user);
      }
      
    }));
  }
  public logout(){
    return this.http.post<any>(Common.baseUrl + 'api/auth/logout','');
  }
  public signup(data: any)
  {
    const body = JSON.stringify(data);
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'True' });
    return this.http.post(Common.baseUrl + 'api/auth/register', body, { headers: reqHeader });
  }

  getUsers(id:number){
    //const reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
    return this.http.get<any>(Common.baseUrl + 'api/Users/?UserId=' + id + '&Email=null&FirstName=null&LastName=null');
  }
  localStorageSet(user: any){
    localStorage.setItem('token', user.token);
    localStorage.setItem('id', user.userId);
    localStorage.setItem('email', user.email);
    localStorage.setItem('firstName', user.firstName);
    localStorage.setItem('lastName', user.lastName);
    this.loginStatus(user.userId, true);
  }
  logOutStatus(){
    const url = Common.baseUrl +'api/LoginStatus';
    const body  = this.setDataForLogStatus('0:0:0:0',0,false);
    return this.http.put(url, body);
  }
  loginStatus(id: any, isLoged: boolean){
    const url = Common.baseUrl +'api/LoginStatus';
    
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'True' });
    
    this.ipService.getIpCliente().subscribe((res:any)=>{  
     const ip = res.ip == undefined ? '0:0:0:0' : res.ip;
     this.setDataForLogStatus(ip,id,isLoged);
    },
    (error)   =>{
      const body  = this.setDataForLogStatus('0:0:0:0',id,isLoged);
      console.log(error);
      return this.http.post(url, body, {headers: reqHeader}).subscribe((p:any) => {
        localStorage.setItem('loginStatus', p.message);
      });
     
    });
    
  }
  setDataForLogStatus(ip, id, isLoged){
    const loginStatusId = localStorage.getItem('loginStatus');
    const data = {
      LoginStatusId: isLoged ? 0 : loginStatusId,
      IpAddress: ip,
      UserId:id,
      IsLoged: isLoged,
      //Date: new Date(),
      //Time: '',
      //LogOutDate: new Date(),
      //LogOutTime: ''
    }

    const body = JSON.stringify(data);
    return body;
  }
}
