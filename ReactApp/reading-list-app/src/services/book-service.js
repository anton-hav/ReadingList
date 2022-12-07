import { environment } from "../environments/environment"

import ApiService from './api-service';
import BookDto from '../dto/book-dto';

export default class BookService {
    constructor(){
        this._apiService = new ApiService();
        this._endpoint = environment.bookEndpoint;
    }

    // READ
    async getBookByIdFromApi(id) {
        let response = await this._apiService.getById(this._endpoint, id);
        let book = BookDto.fromResponse(response);
        return book;
    }

    async isBookExist(title, authorId){
        let response = await this._apiService.get(this._endpoint, {title: title, authorId: authorId});
        for (let i = 0; i < response.length; i++){
            if (response[i].title === title && response[i].authorId === authorId){
                return true;
            };
        }
        return false;
    }

    // CREATE

    async createNewBook(book){

        // let isBookExist = this.isBookExist(book.title, book.authorId);

        // if (isBookExist) {
        //     return "Creation failed. Book already exists.";
        // }
        let result = await this._apiService.post("Books", book);        
        return result;      
    }
}