import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Pet } from "../models/pet";
import { Observable } from "rxjs";

@Injectable()
export class PetsService {
    constructor(
        @Inject('BASE_URL') private baseUrl: string,
        private http: HttpClient) {}

    get(filter: any): Observable<Pet[]> {
        return this.http.get<Pet[]>(`${this.baseUrl}pets`, { params: filter });
    }
}