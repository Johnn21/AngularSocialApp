import { Photo } from "./photo";

export class Member {
    displayName: string;
    userName: string;
    firstName: string;
    lastName: string;
    dateOfBirth: Date;
    gender: string;
    phoneNumber: string;
    description: string;
    photoUrl: string;
    photos: Photo[];
    friendshipRequestStatus: string;
}
