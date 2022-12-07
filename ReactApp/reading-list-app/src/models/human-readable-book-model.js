import Logger from "../utils/logger";
import BookDto from "../dto/book-dto";
import BookNoteDto from "../dto/book-note-dto";
import AuthorDto from "../dto/author-dto";
import CategoryDto from "../dto/category-dto";

export default class HumanReadableBookModel {
  static _logger = Logger.getInstance();
  title = "";
  author = "";
  category = "";
  priority = null;
  status = null;

  constructor(title, author, category, priority, status) {
    this.title = title;
    this.author = author;
    this.category = category;
    this.priority = priority;
    this.status = status;
  }

  static FromParts(bookDto, bookNoteDto, authorDto, categoryDto) {
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
      if (!(categoryDto instanceof CategoryDto)) {
        throw new Error("Invalid category DTO");
      }
      return new HumanReadableBookModel(
        bookDto.title,
        authorDto.fullName,
        categoryDto.name,
        bookNoteDto.priority,
        bookNoteDto.status
      );
    } catch (error) {
      this._logger.error(error.message);
    }
  }
}
