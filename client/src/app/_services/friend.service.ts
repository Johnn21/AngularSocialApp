import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Friend } from '../models/friend';
import { FriendRequestReceive } from '../models/friend-request-receive';
import { PaginatedResult } from '../models/pagination';
import { FriendParams } from '../params/friendParams';

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

  getPaginatedProfileFriendsList(username: string, friendParams: FriendParams) {
    const paginatedResult: PaginatedResult<Friend[]> = new PaginatedResult<Friend[]>();

    let params = new HttpParams();

    params = params.append('pageNumber', friendParams.pageNumber.toString());
    params = params.append('pageSize', friendParams.pageSize.toString());

    return this.http.get<Friend[]>(this.apiUrl + 'friend/get-profile-friends-list/' + username, { observe: 'response', params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        console.log(paginatedResult)
        return paginatedResult;
      })
    )
  }
}
