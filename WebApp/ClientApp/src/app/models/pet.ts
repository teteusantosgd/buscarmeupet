import { PetsFilter } from "./pets-filter";

export interface Pet extends PetsFilter {
    idade: string;
    outrasInformacoes: string;
    image: string;
}