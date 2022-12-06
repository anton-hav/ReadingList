import ApiService from '../api-service';

export default class BookNoteService {
    constructor(){
        this._apiService = new ApiService();
    }

    async isBookNoteExist(bookId){
        let notes = await this._apiService.get("BookNotes", {bookId: bookId});
        for (let i = 0; i < notes.length; i++){
            if (notes[i].bookId === bookId){
                return true;
            };
        }
        return false;
    }

    async createNewBookNote(bookNote){     
        let result = await this._apiService.post("BookNotes", bookNote);        
        return result;      
    }
}