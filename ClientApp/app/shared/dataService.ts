import { HttpClient, HttpHeaders } from "@angular/common/http"
import { Injectable } from "@angular/core"
import { Observable } from "rxjs"
import { map } from "rxjs/operators"
import { Product } from "./product"
import * as OrderNS from './order'


@Injectable()
export class DataService {

    constructor(private http:HttpClient) {

    }

    public token: string = "";
    private tokenExpiration: Date;

    public order: OrderNS.Order = new OrderNS.Order();
    public products: Product[] = [];

    loadProducts(): Observable<boolean> {
        return this.http.get('/api/products')
            .pipe(
                map((data: any[]) => {
                    this.products = data;
                    return true;
                }));

    }

    public get loginRequired(): boolean {
        return this.token.length == 0 || this.tokenExpiration > new Date();
    }

    login(creds: any): Observable<boolean> {
        return this.http
            .post("/account/createtoken", creds)
            .pipe(
             map((data: any) => {
                this.token = data.token;
                this.tokenExpiration = data.expiration;
                return true;
            }));
    }

    public checkout() {

        if (!this.order.orderNumber) { // if new order
            this.order.orderNumber = this.order.orderDate.getFullYear().toString() + this.order.orderDate.getTime().toString();
        }

        return this.http.post("/api/orders", this.order, {
            headers: new HttpHeaders().set("Authorization", "Bearer " + this.token)
        })
            .pipe(
                map(response => {
                    this.order = new OrderNS.Order();
                    return true;
                })
            );
    }

    public AddToOrder(product: Product) {

        let item: OrderNS.OrderItem = this.order.items.find(i => i.productId == product.id);

        if (item) {

            item.quantity++;

        } else {        

        item = new OrderNS.OrderItem();

        item.productId = product.id;
        item.productArtist = product.artist;
        item.productArtId = product.artId;
        item.productCategory = product.category;
        item.productSize = product.size;
        item.productTitle = product.title;
        item.unitPrice = product.price;
            item.quantity = 1; // so that when they add a new product we assume is one 
            this.order.items.push(item);
        }

        
    }
}