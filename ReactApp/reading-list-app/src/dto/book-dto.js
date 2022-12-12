export default class BookDto {
  id = "";
  title = "";
  authorId = "";
  categoryId = "";

  constructor(id, title, authorId, categoryId) {
    this.id = id;
    this.title = title;
    this.authorId = authorId;
    this.categoryId = categoryId;
  }

  static fromResponse(response) {
    return new BookDto(
      response.id,
      response.title,
      response.authorId,
      response.categoryId
    );
  }
}
