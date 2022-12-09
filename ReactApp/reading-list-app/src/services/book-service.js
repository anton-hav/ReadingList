import { environment } from "../environments/environment";

import ApiService from "./api-service";
import BookDto from "../dto/book-dto";
import Logger from "../utils/logger";

export default class BookService {
  constructor() {
    this._apiService = new ApiService();
    this._endpoint = environment.bookEndpoint;
    this._logger = new Logger();
  }

  // READ
  async getBookByIdFromApi(id) {
    let response = await this._apiService.getById(this._endpoint, id);
    let book = BookDto.fromResponse(response);
    return book;
  }

  async isBookExist(title, authorId) {
    let response = await this._apiService.get(this._endpoint, {
      title: title,
      authorId: authorId,
    });
    for (let i = 0; i < response.length; i++) {
      if (response[i].title === title && response[i].authorId === authorId) {
        return true;
      }
    }
    return false;
  }

  /**
   * Gets book specified book note id from api
   * @param {string} bookNoteId - a book note unique identifier
   * @returns book as a BookDto
   */
  async getBookByBookNoteIdFromApi(bookNoteId) {
    try {
      let response = await this._apiService.get(this._endpoint, {
        bookNoteId: bookNoteId,
      });
      let isEmpty = !response.some((book) => book !== undefined);

      if (!isEmpty) {
        let rsp = response.find((book) => book !== undefined)
        let book = BookDto.fromResponse(rsp);
        return book;
      }
      throw new Error("Book not found");
    } catch (error) {
      this._logger.warn(error.message);
    }
  }

  // CREATE

  /**
   * Creates a new book in storage via api
   * @param {Object} book - object with information about the book to be created
   * @returns created book as a BookDto
   */
  async createNewBook(book) {
    try {
        let response = await this._apiService.post("Books", book);
        
        if (response !== undefined) {            
            let book = BookDto.fromResponse(response);
            return book;
        }
        throw new Error("Failed to create book.");
    } catch (error) {
        this._logger.error(error.message);
      }    
  }

  // DELETE

  /**
   * Deletes the book with the specified identifier
   * @param {string} bookId a book unique identifier
   */
  async deleteBookByIdViaApi(bookId){
    try {
        await this._apiService.delete(this._endpoint, bookId);
    } catch (error) {
        this._logger.warn(`${error.message} (for book)`);
    }
  }
}
