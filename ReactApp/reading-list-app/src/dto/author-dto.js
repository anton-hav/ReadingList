export default class AuthorDto {
  id = "";
  fullName = "";

  constructor(id, fullName) {
    this.id = id;
    this.fullName = fullName;
  }

  static fromResponse(response) {
    return new AuthorDto(response.id, response.fullName);
  }
}
