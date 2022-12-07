export default class BookNoteDto {
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

    static fromResponse(response) {
        return new BookNoteDto(
            response.id,
            response.bookId,
            response.priority,
            response.status
        );
    }
}