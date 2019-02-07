    
export class Foo {
    nickname = "type-t"
    private name = "test"
    private age = 0 

    constructor (age: number) {
        this.age = age
    }

    greet() {
        console.log(`hello, I am ${this.name}, ${this.age}.`)
    }
}
