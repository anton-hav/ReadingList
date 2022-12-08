/**
 * Class represents a pagination parameters for requesting to storage
 * @typedef {Object} PaginationParameters
 */
export default class PaginationParameters {
  /**
   * @property {number} pageSize - Number of items per page
   */
  pageSize = null;
  /**
   * @property {number} pageNumber - Current page number
   */
  pageNumber = null;

  /**
   * @property {number} orderBy - Order parameter
   */
  orderBy = null;

  /**
   * @property {bool} isOrderDescending - Flag indicating whether the sorting should be in descending order.
   * It is false by default.
   */
  isOrderDescending = false;

  /**
   * @param {number} pageSize number of items per page
   * @param {number} pageNumber current page number
   * @param {string} pageNumber order parameter as a string
   * @param {string} isOrderDescending order parameter as a string ("desc" or "asc")
   */
  constructor(pageSize, pageNumber, orderBy, isOrderDescending) {
    this.pageSize = pageSize;
    this.pageNumber = pageNumber;

    if (orderBy === "title") {
      this.orderBy = 1;
    } else if (orderBy === "author") {
      this.orderBy = 2;
    } else if (orderBy === "category") {
      this.orderBy = 3;
    } else if (orderBy === "priority") {
      this.orderBy = 4;
    } else if (orderBy === "status") {
      this.orderBy = 5;
    } else {
      this.orderBy = 0;
    }

    if (isOrderDescending === "desc") this.isOrderDescending = true;
  }
}
