import { environment } from "../environments/environment";

import ApiService from "./api-service";
import AuthorService from "./author-service";
import BookService from "./book-service";
import CategoryService from "./category-service";
import BookNoteDto from "../dto/book-note-dto";
import CategoryDto from "../dto/category-dto";
import Model from "../models/human-readable-book-model";
import { PaginationParameters } from "../utils/paginationParameters";
import Logger from "../utils/logger";
import EditBookModel from "../models/edit-book-model";

export default class BookNoteService {
  constructor() {
    this._endpoint = environment.bookNoteEndpoint;
    this._apiService = new ApiService();
    this._bookService = new BookService();
    this._authorService = new AuthorService();
    this._categoryService = new CategoryService();
    this._logger = new Logger();
  }

  // READ
  async getBookNoteByIdFromApi(id) {
    let response = await this._apiService.getById(this._endpoint, id);
    let note = BookNoteDto.fromResponse(response);
    return note;
  }

  async getBookNotesFromApi(parameters) {
    let response = await this._apiService.get(this._endpoint, parameters);
    let notes = response.map((ent) => BookNoteDto.fromResponse(ent));
    return notes;
  }

  /**
   * Counts book notes in storage
   * @returns number of all book notes in storage as a number
   */
  async getBookNoteCountFromApi() {
    let response = await this._apiService.get(
      environment.bookNotesCountEndpoint,
      ""
    );
    return response;
  }

  async getHumanReadableBookNoteByIdFromApi(id) {
    let bookNoteDto = await this.getBookNoteByIdFromApi(id);
    let bookDto = await this._bookService.getBookByIdFromApi(
      bookNoteDto.bookId
    );
    let authorDto = await this._authorService.getAuthorByIdFromApi(
      bookDto.authorId
    );
    let categoryDto = await this._categoryService.getCategoryByIdFromApi(
      bookDto.categoryId
    );
    let model = Model.FromParts(bookDto, bookNoteDto, authorDto, categoryDto);
    return model;
  }

  /**
   * Gets data from the storage and create new an edit book model 
   * specified by book note unique identifier.
   * @param {string} id - a book note unique identifier 
   * @returns an edit book model as a EditBookModel object
   */
  async getEditBookModelByIdFromApi(id){
    let bookNoteDto = await this.getBookNoteByIdFromApi(id);
    let bookDto = await this._bookService.getBookByIdFromApi(
      bookNoteDto.bookId
    );
    let authorDto = await this._authorService.getAuthorByIdFromApi(
      bookDto.authorId
    );    
    let model = EditBookModel.FromParts(bookDto, bookNoteDto, authorDto);
    return model;
  }

  /**
   * Gets book notes from storage with human readable properties
   * @param {PaginationParameters} parameters - pagination parameters
   * @returns {Array} The list of human readable book notes
   */
  async getHumanReadableBooksFromApi(parameters) {
    let notes = await this.getBookNotesFromApi(parameters);
    let models = await Promise.all(
      notes.map(
        async (note) => await this.getHumanReadableBookNoteByIdFromApi(note.id)
      )
    );
    return models;
  }

  async isBookNoteExist(bookId) {
    let notes = await this._apiService.get(this._endpoint, { bookId: bookId });
    for (let i = 0; i < notes.length; i++) {
      if (notes[i].bookId === bookId) {
        return true;
      }
    }
    return false;
  }

  // CREATE
  /**
   * Creates a new book note in storage via api
   * @param {BookNoteDto} bookNote - book note to be created
   * @returns created book as a BookNoteDto
   */
  async createNewBookNote(bookNote) {
    try {
      let response = await this._apiService.post(this._endpoint, bookNote);

      if (response !== undefined) {
        let note = BookNoteDto.fromResponse(response);
        return note;
      }
      throw new Error("Failed to create book note.");
    } catch (error) {
      this._logger.error(error.message);
    }
  }

  // UPDATE

  /**
   * Updates a book note in storage via api.
   * It used patching.
   * @param {BookNoteDto} bookNote - book note to be updated
   */
  async updateBookNote(bookNote) {
    try {
      let response = await this._apiService.patch(
        this._endpoint,
        bookNote,
        bookNote.id        
      );

      if (response !== undefined) {
        let note = BookNoteDto.fromResponse(response);
        return note;
      }
      throw new Error("Failed to update book note.");
    } catch (error) {
      this._logger.error(error.message);
    }
  }

  // DELETE

  /**
   * Deletes the book note with the specified identifier
   * @param {*} bookNoteId - a book note unique identifier
   */
  async deleteBookNoteByIdViaApi(bookNoteId) {
    try {
      await this._apiService.delete(this._endpoint, bookNoteId);
    } catch (error) {
      this._logger.warn(error.message);
    }
  }

  /**
   * Deletes book notes and their corresponding books from the storage
   * @param {Array} bookNoteIds - a list of book note unique identifiers
   */
  async deleteBookNotes(bookNoteIds) {
    for (const id of bookNoteIds) {
      let book = await this._bookService.getBookByBookNoteIdFromApi(id);
      await this.deleteBookNoteByIdViaApi(id);
      await this._bookService.deleteBookByIdViaApi(book.id);
    }
  }
}
