import { environment } from "../environments/environment";

import ApiService from "./api-service";
import AuthorDto from "../dto/author-dto";

export default class AuthorService {
  constructor() {
    this._apiService = new ApiService();
    this._endpoint = environment.authorEndpoint;
  }

  async getAllAuthorsFromApi() {
    let response = await this._apiService.get(this._endpoint, "");
    let authors = response.map(ent => AuthorDto.fromResponse(ent));
    return authors;
  }

  async getAuthorByIdFromApi(id) {
    let response = await this._apiService.getById(this._endpoint, id);
    let author = AuthorDto.fromResponse(response);
    return author;
  }
}
