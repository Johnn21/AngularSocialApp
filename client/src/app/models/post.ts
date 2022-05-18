import { Like } from "./like";

export class Post {
    id: number;
    title: string;
    description: string;
    dateCreated: Date;
    hasPhoto: boolean;
    photoUrl: string;
    username: string;
    userPhoto: string;
    likesCount: number;
    dislikesCount: number;
    likes: Like[] = [];
}