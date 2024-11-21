using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ManufactInfoManager
{
    public static ManufactInfo[] manufactInfos =
    {
        new ManufactInfo(
            new ItemBundle[] { 
                new ItemBundle(ItemIdEnum.MILK_BUCKET, 1, false)},
            new ItemBundle(ItemIdEnum.MILK_PACK, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.MILK_PACK, 1, false),
                new ItemBundle(ItemIdEnum.BANANA, 1, false)},
            new ItemBundle(ItemIdEnum.BANANA_MILK, 3, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.APPLE, 1, false),
                new ItemBundle(ItemIdEnum.CUCUMBER, 1, false),
                new ItemBundle(ItemIdEnum.POTATO, 1, false),
                new ItemBundle(ItemIdEnum.CARROT, 1, false),
                new ItemBundle(ItemIdEnum.BANANA, 1, false)},
            new ItemBundle(ItemIdEnum.FROUT_SALAD, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.CHEESE, 1, false),
                new ItemBundle(ItemIdEnum.UNCOOKED_BEEF, 1, false),
                new ItemBundle(ItemIdEnum.FLOUR, 1, false),
                new ItemBundle(ItemIdEnum.TOMATO, 1, false)},
            new ItemBundle(ItemIdEnum.PIZZA, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.CHEESE, 1, false),
                new ItemBundle(ItemIdEnum.UNCOOKED_PORK, 1, false),
                new ItemBundle(ItemIdEnum.BREAD, 1, false)},
            new ItemBundle(ItemIdEnum.HAMBURGER, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.WATERMELON, 1, false)},
            new ItemBundle(ItemIdEnum.WATERMELON_JUICE, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.HALF_WATERMELON, 1, false)},
            new ItemBundle(ItemIdEnum.WATERMELON_JUICE, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.APPLE, 1, false)},
            new ItemBundle(ItemIdEnum.APPLE_JUICE, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.LEMON, 1, false)},
            new ItemBundle(ItemIdEnum.LEMON_JUICE, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.TOMATO, 1, false)},
            new ItemBundle(ItemIdEnum.TOMATO_JUICE, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.CARROT, 1, false)},
            new ItemBundle(ItemIdEnum.CARROT_JUICE, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.FISH, 1, false),
                new ItemBundle(ItemIdEnum.POTATO, 1, false)},
            new ItemBundle(ItemIdEnum.FISH_AND_CHIPS, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.FLOUR, 1, false),
                new ItemBundle(ItemIdEnum.MILK_PACK, 1, false)},
            new ItemBundle(ItemIdEnum.BREAD, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.SALMON, 1, false),
                new ItemBundle(ItemIdEnum.RICE, 1, false)},
            new ItemBundle(ItemIdEnum.SALMON_SUSHI, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.FROUT_SALAD, 1, false),
                new ItemBundle(ItemIdEnum.UNCOOKED_CHICKEN, 1, false)},
            new ItemBundle(ItemIdEnum.CHICKEN_SALAD, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.COOKED_BEEF, 1, false)},
            new ItemBundle(ItemIdEnum.STEAK, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.FROUT_SALAD, 1, false),
                new ItemBundle(ItemIdEnum.STEAK, 1, false)},
            new ItemBundle(ItemIdEnum.STEAK_SALAD, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.POTATO, 1, false)},
            new ItemBundle(ItemIdEnum.MASHED_POTATO, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.EGG, 1, false),
                new ItemBundle(ItemIdEnum.BREAD, 1, false)},
            new ItemBundle(ItemIdEnum.EGG_TOAST, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.EGG, 1, false),
                new ItemBundle(ItemIdEnum.RICE, 1, false)},
            new ItemBundle(ItemIdEnum.EGG_SUSHI, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.COOKED_BEEF, 1, false),
                new ItemBundle(ItemIdEnum.RICE, 1, false)},
            new ItemBundle(ItemIdEnum.BEEF_SUSHI, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.WATERMELON, 1, false)},
            new ItemBundle(ItemIdEnum.HALF_WATERMELON, 2, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.UNCOOKED_LAMB, 1, false)},
            new ItemBundle(ItemIdEnum.COOKED_LAMB, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.UNCOOKED_PORK, 1, false)},
            new ItemBundle(ItemIdEnum.COOKED_PORK, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.GOLD_ORE, 1, false)},
            new ItemBundle(ItemIdEnum.GOLD_INGOT, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.GOLD_INGOT, 1, false)},
            new ItemBundle(ItemIdEnum.GOLD_NECKLACE, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.SILVER_ORE, 1, false)},
            new ItemBundle(ItemIdEnum.SILVER_INGOT, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.SILVER_INGOT, 1, false)},
            new ItemBundle(ItemIdEnum.SILVER_NECKLACE, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.IRON_ORE, 1, false)},
            new ItemBundle(ItemIdEnum.IRON_INGOT, 1, false)),

        new ManufactInfo(
            new ItemBundle[] {
                new ItemBundle(ItemIdEnum.IRON_INGOT, 1, false)},
            new ItemBundle(ItemIdEnum.IRON_NECKLACE, 1, false)),
    };
}
