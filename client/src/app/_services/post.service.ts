import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post } from '../models/post';
import { PostComment } from '../models/post-comment';

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

  addLikeToPost(postId: number) {
    return this.http.post<Post>(this.apiUrl + 'post/add-like-to-post/' + postId, {});
  }

  addDislikeToPost(postId: number) {
    return this.http.post<Post>(this.apiUrl + 'post/add-dislike-to-post/' + postId, {});
  }

  addPostComment(postId: number, content: string) {
    return this.http.post<PostComment>(this.apiUrl + 'post/add-comment-to-post', {postId, content});
  }

  getPostComments(postId: number, skipPostComments: number) {
    return this.http.get<PostComment[]>(this.apiUrl + 'post/get-post-comments/' + postId + '/' + skipPostComments);
  }
}
