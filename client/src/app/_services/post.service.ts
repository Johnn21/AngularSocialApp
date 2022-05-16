import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post } from '../models/post';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addPost(model: any) {
    return this.http.post(this.apiUrl + 'post/add-post', model);
  }

  getFriendsPost(skipPosts: number) {

    let params = new HttpParams();
    params = params.append('skipPosts', skipPosts);

    return this.http.get<Post[]>(this.apiUrl + 'post/get-friends-posts', {params});
  }
}
