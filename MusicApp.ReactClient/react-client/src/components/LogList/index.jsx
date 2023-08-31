export function LogList({ list }) {
    return <div className="d-flex justify-content-center">
        <div>
            {list.map((message, i) => {

                var color = 'text-secondary';

                if(message.split(' ')[1] === 'added')
                {
                    color = 'text-primary';
                }

                return (
                    <div>
                        <span className={color}>{message}</span>
                    </div>
                )
            })}
        </div> 
    </div>
}