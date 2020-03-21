const baseURL = "http://localhost:54083/api/Contacts";

const resolveApiURL = (action, value = "") => {
    switch (action) {
        case "getWithQuery":
            return baseURL + `?searchQuery=${value}`;       
        default:
            break;
    }
}

export { resolveApiURL };