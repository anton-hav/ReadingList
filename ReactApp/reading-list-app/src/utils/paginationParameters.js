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
   * @param {number} pageSize number of items per page
   * @param {number} pageNumber current page number
   */
  constructor(pageSize, pageNumber) {
    this.pageSize = pageSize;
    this.pageNumber = pageNumber;
  }
}
