import { environment } from "../environments/environment";

export default class ApiService {
  // should be private
  _getFullUrl(url) {
    return environment.apiUrl + url;
  }

  async get(url, data = {}) {
    let fullUrl = new URL(`${this._getFullUrl(url)}`);
    if (Object.keys(data).length > 0) {
      fullUrl.search = new URLSearchParams(Object.entries(data)).toString();
      //Object.keys(data).forEach(key => fullUrl.searchParams.append(key, data[key]));
    }

    let response = await fetch(fullUrl);
    return response.json();
  }

  async getById(url, id) {
    let fullUrl = new URL(`${this._getFullUrl(url)}/${id}`);

    let response = await fetch(fullUrl);
    return response.json();
  }

  async post(url, data) {
    let fullUrl = new URL(`${this._getFullUrl(url)}`);

    let response = await fetch(fullUrl, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });
    return response.json();
  }

  put() {}

  patch() {}

  async delete(url, id) {
    let fullUrl = new URL(`${this._getFullUrl(url)}/${id}`);

    let response = await fetch(fullUrl, {
      method: "DELETE",
    });
    if (response.ok)
      return true;
    
    throw new Error("Failed to delete");    
  }
}
