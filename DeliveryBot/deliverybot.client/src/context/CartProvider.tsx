import React, { ReactNode, createContext, useState } from 'react';
import { CartProduct } from '../interfaces/product';

interface CartContextProps {
  cart: CartProduct[];
  addToCart: (item: CartProduct) => void;
  removeFromCart: (productId: string) => void;
  clearCart: () => void;
}

interface CartProviderProps {
  children: ReactNode;
}

const CartContext = createContext({} as CartContextProps);

export const CartProvider: React.FC<CartProviderProps> = ({ children }) => {
  const [cart, setCart] = useState<CartProduct[]>([]);

  const addToCart = (item: CartProduct) => {
    setCart((prevCart) => [...prevCart, item]);
  };

  const removeFromCart = (productId: string) => {
    setCart((prevCart) => prevCart.filter((item) => item.id !== productId));
  };

  const clearCart = () => {
    setCart([]);
  };

  return (
    <CartContext.Provider value={{ cart, addToCart, removeFromCart, clearCart }}>
      {children}
    </CartContext.Provider>
  );
};

export default CartContext;
