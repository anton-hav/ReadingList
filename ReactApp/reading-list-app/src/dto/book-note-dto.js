import Logger from "../utils/logger";
import HumanReadableBookModel from "../models/human-readable-book-model";

export default class BookNoteDto {
  static _logger = Logger.getInstance();
  id = "";
  bookId = "";
  priority = null;
  status = null;

  constructor(id, bookId, priority, status) {
    this.id = id;
    this.bookId = bookId;
    this.priority = priority;
    this.status = status;
  }

  /**
   * Mapping from api response (as an json) to BookNoteDto
   * @param {*} response - response object as a json
   * @returns new BookNoteDto
   */
  static fromResponse(response) {
    return new BookNoteDto(
      response.id,
      response.bookId,
      response.priority,
      response.status
    );
  }

  /**
   * Mapping from human readable book model and book id to BookNoteDto
   * @param {HumanReadableBookModel} model - human readable book model
   * @param {string} bookId - book unique identifier as a string
   * @returns new BookNoteDto
   */
  static fromHumanReadableBookModelAndBookId(model, bookId) {
    try {
      if (!(model instanceof HumanReadableBookModel)) {
        throw new Error("Invalid book model");
      }
      if (bookId === undefined) {
        throw new Error("Invalid book id");
      }
      return new BookNoteDto(model.id, bookId, model.priority, model.status);
    } catch (error) {
      this._logger.error(error.message);
    }
  }
}
