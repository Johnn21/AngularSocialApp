import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addPost(model: any) {
    return this.http.post(this.apiUrl + 'post/add-post', model);
  }
}