import { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [suppliers, setSuppliers] = useState();
    const [poundsAvailableNeeded, setPoundsAvailableNeeded] = useState(0);

    const handleInputChange = (event) => {
        setPoundsAvailableNeeded(event.target.value);
    };

    async function getPotatoSuppliers() {
        const response = await fetch(`potatopricefinder${poundsAvailableNeeded ? `?pounds=${poundsAvailableNeeded}` : ''}`, { method: 'get' });
        const data = await response.json();
        setSuppliers(data);
    }

    useEffect(() => {
        getPotatoSuppliers();
    }, []);

    const contents = suppliers === undefined
        ? <p><em>Loading... </em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Price</th>
                    <th>Bag weight (pounds)</th>
                    <th>Available bags</th>
                    <th>Price per pound</th>
                   
                </tr>
            </thead>
            <tbody>
                {suppliers.map(supplier =>
                    <tr key={supplier.name}>
                        <td>{supplier.name}</td>
                        <td>{supplier.price}</td>
                        <td>{supplier.bagWeight}</td>
                        <td>{supplier.bagAvailable}</td>
                        <td>{supplier.pricePerPound}</td>
                       
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tableLabel">Potato Suppliers</h1>
            <div className="flex">
                <label htmlFor="pounds">Pounds of potatoes needed:</label>
                <div>
                <input
                    type="number"
                    id="pounds"
                        value={poundsAvailableNeeded}
                        onChange={handleInputChange}
                />
                    <button onClick={getPotatoSuppliers}>Search</button>
                </div>
                </div>
           {contents}
        </div>
    );
}

export default App;