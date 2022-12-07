import { environment } from "../environments/environment";

import ApiService from "./api-service";
import AuthorService from "./author-service";
import BookService from "./book-service";
import CategoryService from "./category-service";
import BookNoteDto from "../dto/book-note-dto";
import CategoryDto from "../dto/category-dto";
import Model from "../models/human-readable-book-model";

export default class BookNoteService {
  constructor() {
    this._endpoint = environment.bookNoteEndpoint;
    this._apiService = new ApiService();
    this._bookService = new BookService();
    this._authorService = new AuthorService();
    this._categoryService = new CategoryService();
  }

  // READ
  async getBookNoteByIdFromApi(id) {
    let response = await this._apiService.getById(this._endpoint, id);
    let note = BookNoteDto.fromResponse(response);
    return note;
  }

  async getBookNotesFromApi() {
    let response = await this._apiService.get(this._endpoint, "");
    let notes = response.map(ent => BookNoteDto.fromResponse(ent));
    return notes;
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

  async getHumanReadableBooksFromApi() {
    let notes = await this.getBookNotesFromApi();
    let models = await Promise.all(notes.map(async note => await this.getHumanReadableBookNoteByIdFromApi(note.id)));
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
  async createNewBookNote(bookNote) {
    let result = await this._apiService.post(this._endpoint, bookNote);
    return result;
  }
}
