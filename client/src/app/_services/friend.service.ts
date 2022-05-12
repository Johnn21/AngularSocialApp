import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Friend } from '../models/friend';
import { FriendRequestReceive } from '../models/friend-request-receive';

@Injectable({
  providedIn: 'root'
})
export class FriendService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  sendFriendshipRequest(receiverUsername: string) {
    return this.http.post(this.apiUrl + 'friend/send-friendship-request/' + receiverUsername, {});
  }

  getFriendshipRequests(username: string) {
    return this.http.get<FriendRequestReceive[]>(this.apiUrl + 'friend/get-friendship-requests/' + username);
  }

  addFriendship(username: string) {
    return this.http.post(this.apiUrl + 'friend/add-friendship/' + username, {});
  }

  rejectFriendship(username: string) {
    return this.http.delete(this.apiUrl + 'friend/reject-friendship/' + username);
  }

  getFriendsList() {
    return this.http.get<Friend[]>(this.apiUrl + 'friend/get-friends-list');
  }
}
