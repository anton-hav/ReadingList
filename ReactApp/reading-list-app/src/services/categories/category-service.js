import ApiService from '../api-service';

export default class CategoryService {
    constructor(){
        this._apiService = new ApiService();
    }
    
    async getAllCategoriesFromApi() {
        let categories = await this._apiService.get("Categories", "");
        return categories;
    }
}