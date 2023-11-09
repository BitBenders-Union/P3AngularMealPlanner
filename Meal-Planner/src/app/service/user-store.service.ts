import { Injectable } from '@angular/core';
import {BehaviorSubject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserStoreService {
  private user$ = new BehaviorSubject<string>("");
  private id$ = new BehaviorSubject<number>(0);
  private counter: number = 0;

  constructor() { }

  public getUserFromStore(){

    return this.user$.asObservable();
  }

  public getUserFromStoreTest(sender: string){
    this.counter++;
    console.log(`From ${sender}, counter: `, this.counter)
    return this.user$.asObservable();
  }

  public setUserInStore(user: string){
    this.user$.next(user);
  }

  public getIdFromStore(){
    return this.id$.asObservable();
  }

  public setIdInStore(id: number){
    this.id$.next(id);
  }


}
