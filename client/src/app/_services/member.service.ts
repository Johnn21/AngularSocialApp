import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMemberByUsername(username: string) {
    return this.http.get<Member>(this.apiUrl + 'member/getMemberByUsername/' + username);
  }

  updateUser(model: any) {
    return this.http.put(this.apiUrl + 'member', model);
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.apiUrl + 'member/delete-photo/' + photoId);
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.apiUrl + 'member/set-main-photo/' + photoId, {});
  }
}
