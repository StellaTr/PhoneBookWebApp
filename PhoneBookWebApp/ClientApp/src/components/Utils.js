const baseURL = "http://localhost:54083/api/Contacts";

const resolveApiURL = (action, value = "") => {
    switch (action) {
        case "getWithQuery":
            return baseURL + `?searchQuery=${value}`;       
        case "post":
            return baseURL;
        default:
            break;
    }
}

const isEmpty = (object) => Object.values(object).some(x => x.toString().trim() == '');

const isPhoneValid = (phone) => {
    const digits = /^[0-9]+$/;

    if (!phone.countryCode.match(digits) || !phone.areaCode.match(digits))
        return false;

    if (!phone.phoneNumber.match(digits) || phone.phoneNumber.length < 6)
        return false;

    return true;
};

export { resolveApiURL, isEmpty, isPhoneValid };