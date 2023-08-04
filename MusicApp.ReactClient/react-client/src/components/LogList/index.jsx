import Log from "../Log"

export function LogList({ list }) {
    return <div className="d-flex justify-content-center">
        <div>
            {list.map((message, i) => {
                return (
                    <Log message={message} key={i} />
                )
            })}
        </div> 
    </div>
}