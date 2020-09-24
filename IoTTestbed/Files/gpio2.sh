#!/bin/bash

PINS=( 17 4 18 27 22 23 24 25 16 12 20 21 5 6 )
OUTPUTS=( 16 12 20 21 20 5 6)
INVERTED=( 20 21 5 )

for PIN in "${PINS[@]}"
do
if [ ! -d /sys/class/gpio/gpio$PIN ]; then
echo $PIN | sudo tee /sys/class/gpio/export > /dev/null
fi
echo in | sudo tee /sys/class/gpio/gpio$PIN/direction > /dev/null
done

for OUTPUT in "${OUTPUTS[@]}"
do
echo out | sudo tee /sys/class/gpio/gpio$OUTPUT/direction > /dev/null
done


for INVERT in "${INVERTED[@]}"
do
echo 1 | sudo tee /sys/class/gpio/gpio$INVERT/value > /dev/null
doneif [ "$1" == "0" ] || [ "$1" == "1" ]; then

if [ ! -d /sys/class/gpio/gpio21 ]; then
echo "21" | sudo tee /sys/class/gpio/export &> /dev/null
fi

#remove this check to force toggle
if grep -v -q out /sys/class/gpio/gpio21/direction; then
echo "out" | sudo tee /sys/class/gpio/gpio21/direction > /dev/null
fi
echo "$1" | sudo tee /sys/class/gpio/gpio21/value > /dev/null

else
echo "usage: $0 [0,1]"
fi