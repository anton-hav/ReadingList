import Logger from "../utils/logger";
import BookDto from "../dto/book-dto";
import BookNoteDto from "../dto/book-note-dto";
import AuthorDto from "../dto/author-dto";
import CategoryDto from "../dto/category-dto";
import HumanReadableBookModel from "./human-readable-book-model";

export default class EditBookModel {
  static _logger = Logger.getInstance();
  /**
   * @property {string} id - Unique identifier.
   * It equals the unique identifier of the book notes in the storage.
   */
  id = "";
  title = "";
  author = "";
  categoryId = "";
  priority = null;
  status = null;

  constructor(id, title, author, categoryId, priority, status) {
    this.id = id;
    this.title = title;
    this.author = author;
    this.categoryId = categoryId;
    this.priority = priority;
    this.status = status;
  }

  static FromParts(bookDto, bookNoteDto, authorDto) {
    try {
      if (!(bookDto instanceof BookDto)) {
        throw new Error("Invalid book DTO");
      }
      if (!(bookNoteDto instanceof BookNoteDto)) {
        throw new Error("Invalid book note DTO");
      }
      if (!(authorDto instanceof AuthorDto)) {
        throw new Error("Invalid author DTO");
      }
      return new EditBookModel(
        bookNoteDto.id,
        bookDto.title,
        authorDto.fullName,
        bookDto.categoryId,
        bookNoteDto.priority,
        bookNoteDto.status
      );
    } catch (error) {
      this._logger.error(error.message);
    }
  }

  static fromHumanReadableBookModelAndCategoryId(model, categoryId) {
    try {
      if (!(model instanceof HumanReadableBookModel)) {
        throw new Error("Invalid human readable book model");
      }      
      return new EditBookModel(
        model.id,
        model.title,
        model.fullName,
        categoryId,
        model.priority,
        model.status
      );
    } catch (error) {
      this._logger.error(error.message);
    }
  }

  static clone (book){
    try {
      if (!(book instanceof EditBookModel)) {
        throw new Error("Invalid edit book model for clone.");
      }

      let clone = new EditBookModel();

      for (let key in book) {
        clone[key] = book[key];
      }
      return clone;
      
    } catch (error) {
      this._logger.error(error.message);
    }
  }
}
