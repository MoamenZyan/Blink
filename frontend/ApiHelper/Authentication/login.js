
// Login api
const baseURL = "http://localhost:8080/api/v1";
export default function Login(data) {
    const formData = new URLSearchParams(data);
    return fetch(`${baseURL}/login`, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded",
        },
        body: formData
    }).then(res => {
        if (res.ok)
            return true;
        else
            return false;
    })
}
