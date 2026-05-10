export interface CartItem {
  id: number;
  productId: number;
  productName: string;
  price: number;
  imageUrl: string | null;
  quantity: number;
  addedAt: string;
}
