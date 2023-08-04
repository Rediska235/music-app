export function Welcome({ username }) {
    return <div className="d-flex justify-content-center">
        <h1>Welcome, {username === '' ? 'guest' : username}!</h1>
    </div>
}

