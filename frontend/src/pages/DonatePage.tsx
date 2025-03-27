import { useNavigate, useParams } from 'react-router-dom';
import WelcomeBand from '../components/WelcomeBand';
import { useCart } from '../context/CartContext';
import { useState } from 'react';
import { CartItem } from '../types/CartItem';

function DonatePage() {
  const navigate = useNavigate();
  const { projectName, projectId } = useParams();
  const { addToCart } = useCart();
  const [donationAmount, setDonationAmount] = useState<number>(0);

  const handleAddToCart = () => {
    const newItem: CartItem = {
      projectId: Number(projectId),
      projectName: projectName || 'No Project Found',
      donationAmount,
    };

    addToCart(newItem);
    navigate('/cart');
  };

  return (
    <>
      <WelcomeBand />
      <h2>Donate to {projectName}</h2>
      <div>
        <input
          type="number"
          min="0"
          placeholder="Enter Donation Amount"
          value={donationAmount}
          onChange={(x) => setDonationAmount(Number(x.target.value))}
        />
        <button onClick={handleAddToCart}>Add to Cart</button>
      </div>
      <button className="btn btn-success" onClick={() => navigate('/projects')}>
        Home
      </button>
      {/* You can also use navigate(-1) to just go back to the previous page
      from the one you're on */}
    </>
  );
}

export default DonatePage;
