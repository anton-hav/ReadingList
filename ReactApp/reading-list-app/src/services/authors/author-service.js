import ApiService from '../api-service';

export default class AuthorService {
    constructor(){
        this._apiService = new ApiService();
    }
    
    async getAllAuthorsFromApi() {
        let authors = await this._apiService.get("Authors", "");
        return authors;
    }

    async getAuthorByIdFromApi(id) {
        let author = await this._apiService.getById("Authors", id);
        return author;
    }
}