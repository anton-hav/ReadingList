export default class CategoryDto {
  id = "";
  name = "";

  constructor(id, name) {
    this.id = id;
    this.name = name;
  }

  static fromResponse(response) {
    return new CategoryDto(response.id, response.name);
  }
}
