import ApiService from '../api-service';

export default class BookService {
    constructor(){
        this._apiService = new ApiService();
    }

    async isBookExist(title, authorId){
        let books = await this._apiService.get("Books", {title: title, authorId: authorId});
        for (let i = 0; i < books.length; i++){
            if (books[i].title === title && books[i].authorId === authorId){
                return true;
            };
        }
        return false;
    }

    async createNewBook(book){

        // let isBookExist = this.isBookExist(book.title, book.authorId);

        // if (isBookExist) {
        //     return "Creation failed. Book already exists.";
        // }
        let result = await this._apiService.post("Books", book);        
        return result;      
    }
}