const start = 100;
const finish = 300000;

// Start time
const t0 = performance.now();

for (let i = start; i <= finish; i++) {
    let isPrime = true;

    for (let j = 2; j * j <= i; j++) {
        if (i % j === 0) {
            isPrime = false;
            break;
        }
    }

    if (i > 1 && isPrime) {
        console.log(i);
    }
}

// End time
const t1 = performance.now();

console.log(`Time taken: ${(t1 - t0).toFixed(2)} milliseconds`);
