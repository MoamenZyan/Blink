
const baseURL = "http://localhost:8080/api/v1";
export default function Logout() {
    return fetch(`${baseURL}/user/logout`, {
        method: "GET",
        credentials: "include"
    }).then(res => {
        if (res.ok)
            return true;
        else
            return false;
    })
}
