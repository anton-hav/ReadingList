import { environment } from "../environments/environment";

import ApiService from "./api-service";
import AuthorDto from "../dto/author-dto";
import Logger from "../utils/logger";

export default class AuthorService {
  constructor() {
    this._apiService = new ApiService();
    this._endpoint = environment.authorEndpoint;
    this._logger = new Logger();
  }

  // READ

  /**
   * Gets all authors from the storage
   * @returns a list of authors as an array of AuthorDto objects
   */
  async getAllAuthorsFromApi() {
    let response = await this._apiService.get(this._endpoint, "");
    let authors = response.map(ent => AuthorDto.fromResponse(ent));
    return authors;
  }

  /**
   * Gets the author specified by the unique identifier
   * @param {string} id - an author unique identifier
   * @returns an author as an AuthorDto object
   */
  async getAuthorByIdFromApi(id) {
    let response = await this._apiService.getById(this._endpoint, id);
    let author = AuthorDto.fromResponse(response);
    return author;
  }

  // CREATE

  async createNewAuthor(authorDto) {
    try {
      let response = await this._apiService.post(this._endpoint, authorDto);

      if (response !== undefined) {
        let result = AuthorDto.fromResponse(response);
        return result;
      }
      throw new Error("Failed to create new author");
    } catch (error) {
      this._logger.error(error.message);
    }
  }
}
