import { environment } from "../environments/environment"


export default class ApiService {

    // should be private
    getFullUrl(url) {
        return environment.apiUrl + url;
    }

    async get(url, data = {}){
        let fullUrl = new URL(`${this.getFullUrl(url)}`);
        if (Object.keys(data).length > 0) {
            fullUrl.search = new URLSearchParams(Object.entries(data)).toString();
            //Object.keys(data).forEach(key => fullUrl.searchParams.append(key, data[key]));
        }
    
        let response = await fetch(fullUrl);
        return response.json();
    }

    async getById(url, id){
        let fullUrl = new URL(`${this.getFullUrl(url)}/${id}`);
    
        let response = await fetch(fullUrl);
        return response.json();
    }

    async post(url, data){
        let fullUrl = new URL(`${this.getFullUrl(url)}`);
    
        let response = await fetch(fullUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'                
              },
            body: JSON.stringify(data)
        });
        return response.json();
    }

    put(){

    }

    patch(){

    }

    delete(){

    }
}